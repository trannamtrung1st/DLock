import { Button, Card, Col, Divider, Row, Typography } from 'antd';
import React, { useState, useEffect, useContext, useCallback } from 'react';
import AppSpin from '@shared/components/app-spin';
import { FieldModel } from '@app/models/field.model';
import { bookingService, fieldService } from '@app/services';
import { GlobalContext } from '@app/contexts/global.context';
import { BookingModel } from '@app/models/booking.model';
import { joinMessages, showErrorMessage } from '@shared/helpers/messages.helper';
import { groupBy } from 'lodash';
import { formatClientDateTime } from '@shared/helpers/formatting.helper';

const { Text } = Typography;

const BookingPage = () => {
  const { userName } = useContext(GlobalContext) || {};
  const [fields, setFields] = useState<FieldModel[]>([]);
  const [bookings, setBookings] = useState<BookingModel[]>([]);

  useEffect(() => {
    if (!userName) return;
    bookingService.getBookings(userName).then(result => {
      if (result.response.ok && result.apiData?.data) {
        setBookings(result.apiData.data);
      }
    })
  }, [userName]);

  useEffect(() => {
    fieldService.getAllFields().then(result => {
      if (result.response.ok && result.apiData?.data) {
        setFields(result.apiData.data);
      }
    });
  }, []);

  const onBookCallback = useCallback((fieldName: string) => () => {
    if (!userName) return;
    bookingService.creatBooking({ fieldName, userName }).then(result => {
      if (result.response.ok && result.apiData?.data) {
        const booking = result.apiData?.data;
        setBookings(prev => [booking, ...prev]);
      } else {
        showErrorMessage(joinMessages(result.apiData?.messages));
      }
    });
  }, [userName]);

  return (
    <Card title={
      <Row align="middle">
        <Col flex="none">Booking</Col>
      </Row>
    } bordered={false}>
      <AppSpin spinning={false}>
        <Row gutter={[16, 16]}>
          {fields.map((f, idx) => <Col key={idx} span={4}>
            <Button style={{ width: '100%' }}
              onClick={onBookCallback(f.name)}
              type='primary'>Book: {f.name}
            </Button>
          </Col>)}
        </Row>
        <Divider orientation='left'>Booking history</Divider>
        <Row gutter={[16, 16]} style={{ maxHeight: 200, overflow: 'auto' }}>
          {bookings.map((b, idx) => <Col key={idx} span={24}>
            <Text>Booked {b.fieldName} at {formatClientDateTime(b.bookedTime)} - Server: {b.bookedFromServerId}</Text>
          </Col>)}
        </Row>
        <Divider orientation='left'>Booking count</Divider>
        <Row gutter={[16, 16]}>
          {Object.entries(groupBy(bookings, b => b.userName))
            .map(([userName, bookings], idx) => ({ userName, bookingCount: bookings.length }))
            .map((o, idx) => <Col key={idx} span={24}>
              <Text><Text strong>{o.userName}</Text> - Booking count: {o.bookingCount}</Text>
            </Col>)}
        </Row>
      </AppSpin>
    </Card>
  )
};

export default BookingPage;
