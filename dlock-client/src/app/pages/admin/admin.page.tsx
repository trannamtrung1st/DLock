import { Button, Card, Col, Divider, Form, Modal, Row, Select, Space, Typography } from 'antd';
import React, { useEffect, useState, useContext, useCallback } from 'react';
import AppSpin from '@shared/components/app-spin';
import { GlobalContext } from '@app/contexts/global.context';
import { isAdmin } from '@shared/constants/accounts.const';
import { Navigate } from 'react-router-dom';
import { routeMap } from '@shared/constants/routes.const';
import bookingService from '@app/services/booking.service';
import { joinMessages, showErrorMessage, showSuccessMessage } from '@shared/helpers/messages.helper';
import { adminService, fieldService, identityService } from '@app/services';
import { BookingModel } from '@app/models/booking.model';
import { FieldModel } from '@app/models/field.model';
import { groupBy } from 'lodash';
import { antdModal } from '@shared/components/antd-functions';
import { appMessages } from '@shared/constants/messages.const';
import { UserModel } from '@app/models/user.model';
import {
  ReloadOutlined
} from '@ant-design/icons';
import { formatClientDateTime } from '@shared/helpers/formatting.helper';

const { Text } = Typography;

const AdminPage = () => {
  const { userName, logout } = useContext(GlobalContext) || {};
  const [bookings, setBookings] = useState<BookingModel[]>([]);
  const [fields, setFields] = useState<FieldModel[]>([]);
  const [users, setUsers] = useState<UserModel[]>([]);
  const [maintenanceStatusModalVisible, setMaintenanceStatusModalVisible] = useState(false);

  const getBookings = () => bookingService.getAllBookings().then(result => {
    if (result.response.ok && result.apiData?.data) {
      setBookings(result.apiData.data);
    }
  });

  const getUsers = () => identityService.getAllUsers().then(result => {
    if (result.response.ok && result.apiData?.data) {
      setUsers(result.apiData.data);
    }
  });

  const getFields = () => fieldService.getAllFields().then(result => {
    if (result.response.ok && result.apiData?.data) {
      setFields(result.apiData.data);
    }
  });

  useEffect(() => {
    getUsers();
  }, []);

  useEffect(() => {
    getBookings();
  }, []);

  useEffect(() => {
    getFields();
  }, []);

  const onChangeStatusSuccess = (model: any) => {
    getFields();
    setMaintenanceStatusModalVisible(false);
  };

  const warning = (onOk: () => void) => {
    const { destroy } = antdModal.warning({
      title: appMessages.common.beCareful,
      content: appMessages.common.confirm,
      okCancel: true,
      cancelText: appMessages.common.back,
      okText: appMessages.common.confirm,
      onCancel: () => destroy(),
      onOk: onOk
    });
  }

  const onResetBookingData = () => {
    warning(() => {
      bookingService.resetData().then((result) => {
        if (result.response.ok) {
          showSuccessMessage('Reset booking data successfully!');
          getBookings();
        } else {
          showErrorMessage(joinMessages(result.apiData?.messages));
        }
      });
    });
  };

  const onResetFieldData = () => {
    warning(() => {
      fieldService.resetData().then((result) => {
        if (result.response.ok) {
          showSuccessMessage('Reset field data successfully!');
          getFields();
        } else {
          showErrorMessage(joinMessages(result.apiData?.messages));
        }
      });
    })
  };

  const onResetIdentityData = () => {
    warning(() => {
      identityService.resetData().then((result) => {
        if (result.response.ok) {
          showSuccessMessage('Reset identity data successfully!');
          logout && logout();
        } else {
          showErrorMessage(joinMessages(result.apiData?.messages));
        }
      });
    })
  };

  const onChangeProxyConfig = useCallback((configName: string) => () => {
    warning(() => {
      adminService.changeProxyConfig(configName).then((result) => {
        if (result.response.ok) {
          showSuccessMessage('Changed successfully!');
        } else {
          showErrorMessage(joinMessages(result.apiData?.messages));
        }
      });
    })
  }, []);

  const onChangeLockService = useCallback((serviceName: string) => () => {
    warning(() => {
      adminService.changeLockService(serviceName).then((result) => {
        if (result.response.ok) {
          showSuccessMessage('Changed successfully!');
        } else {
          showErrorMessage(joinMessages(result.apiData?.messages));
        }
      });
    })
  }, []);

  if (!userName || !isAdmin(userName)) {
    return <Navigate to={routeMap.home.link} />
  }

  return (
    <>

      <Card title={
        <Row align="middle">
          <Col flex="none">Admin</Col>
        </Row>
      } bordered={false}>
        <AppSpin spinning={false}>
          <Space direction="vertical">
            <Row gutter={[16, 16]}>
              <Col><Button onClick={onChangeProxyConfig('SingleServer')} type="primary">Use single Booking server</Button></Col>
              <Col><Button onClick={onChangeProxyConfig('MultipleServer')} type="primary">Use multiple Booking servers</Button></Col>
            </Row>
            <Row gutter={[16, 16]}>
              <Col><Button onClick={onChangeLockService('No lock')} type="primary">Disable lock</Button></Col>
              <Col><Button onClick={onChangeLockService('Local lock')} type="primary">Use local lock</Button></Col>
              <Col><Button onClick={onChangeLockService('Redis lock')} type="primary">Use Redis lock</Button></Col>
              <Col><Button onClick={onChangeLockService('Red lock')} type="primary">Use Red lock</Button></Col>
            </Row>
            <Row gutter={[16, 16]}>
              <Col><Button onClick={onResetBookingData} danger type="primary">Reset booking data</Button></Col>
              <Col><Button onClick={onResetFieldData} danger type="primary">Reset field data</Button></Col>
              <Col><Button onClick={onResetIdentityData} danger type="primary">Reset identity data</Button></Col>
            </Row>
          </Space>
          <Divider orientation='left'>Field list</Divider>
          <Row gutter={[8, 8]} style={{ maxHeight: 200, overflow: 'auto' }}>
            <Col span={24}>
              <Button onClick={() => setMaintenanceStatusModalVisible(true)}
                type="primary">
                Change maintenance status
              </Button>
            </Col>
            {fields.map((f, idx) => <Col key={idx} span={8}>
              <Text><Text strong>{f.name}</Text> - {f.isUnderMaintenance ? 'Under maintenance' : 'Good condition'}</Text>
              <br />
              {f.maintenanceTime && <Text>Maintenance time: {formatClientDateTime(f.maintenanceTime)}</Text>}
            </Col>)}
          </Row>
          <Divider orientation='left'>Booking history</Divider>
          <Row gutter={[16, 16]} style={{ maxHeight: 100, overflow: 'auto' }}>
            {bookings.map((b, idx) => <Col key={idx} span={24}>
              <Text><Text strong>{b.userName}</Text> booked {b.fieldName} at {formatClientDateTime(b.bookedTime)} - Server: {b.bookedFromServerId}</Text>
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
          <Divider orientation='left'>
            Users <ReloadOutlined onClick={() => getUsers()} style={{ fontSize: 14, cursor: 'pointer' }} />
          </Divider>
          <Row gutter={[16, 16]}>
            {users.map((u, idx) => <Col key={idx}>
              <Text>{u.userName}</Text>
            </Col>)}
          </Row>
        </AppSpin>
      </Card>

      {maintenanceStatusModalVisible && <ChangeMaintenanceStatusModal
        fields={fields}
        onCancel={() => setMaintenanceStatusModalVisible(false)}
        onSuccess={onChangeStatusSuccess}
        onFinishFailed={(error) => console.log(error)}
        open={maintenanceStatusModalVisible}
      />}
    </>
  )
};

export default AdminPage;

const ChangeMaintenanceStatusModal = ({
  open, onCancel, onSuccess, onFinishFailed, fields
}: {
  open: boolean,
  onCancel: () => void,
  onSuccess: (model: { fieldName: string, value: boolean }) => void,
  onFinishFailed: (error: any) => void,
  fields: FieldModel[]
}) => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);

  const handleFinish = (model: { fieldName: string, value: boolean }) => {
    console.log(model);
    setLoading(true);
    const { fieldName, value } = model;
    fieldService.changeMaintenanceStatus(fieldName, value).then(result => {
      if (result.response.ok) {
        showSuccessMessage('Succesfully changed status!');
        onSuccess(model);
      } else {
        showErrorMessage(joinMessages(result.apiData?.messages))
      }
    });
  }

  return (
    <Modal open={open} title={'Change maintenance status'} footer={<></>}
      onCancel={onCancel}
    >
      <Form
        layout="vertical"
        form={form}
        onFinish={handleFinish}
        onFinishFailed={onFinishFailed}
        autoComplete="off"
        disabled={loading}
      >
        <Row gutter={[16, 16]}>
          <Col xs={24} md={12}>
            <Form.Item label="Field" name="fieldName">
              <Select
                options={fields.map(f => ({ value: f.name, label: f.name }))}
              />
            </Form.Item>
          </Col>
          <Col xs={24} md={12}>
            <Form.Item label="Status" name="value">
              <Select
                options={[
                  { value: 'true', label: 'Under maintenance' },
                  { value: 'false', label: 'Good condition' }
                ]}
              />
            </Form.Item>
          </Col>
        </Row>
        <Row justify="end" gutter={[8, 8]}>
          <Col>
            <Form.Item
              noStyle
              shouldUpdate
            >
              {({ getFieldsError }) => {
                const disabled = getFieldsError().some(f => f.errors.length > 0);
                return (
                  <Button type="primary" htmlType="submit"
                    disabled={disabled}
                    loading={loading}>
                    Submit
                  </Button>
                );
              }}
            </Form.Item>
          </Col>
        </Row>
      </Form>
    </Modal>
  )
}