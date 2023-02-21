import { Button, Card, Col, Form, Input, Row } from 'antd';
import React, { useContext, useEffect, useState } from 'react';
import AppSpin from '@shared/components/app-spin';
import { GlobalContext } from '@app/contexts/global.context';
import { isAdmin } from '@shared/constants/accounts.const';
import { adminService } from '@app/services';
import { joinMessages, showErrorMessage, showSuccessMessage } from '@shared/helpers/messages.helper';

const SharingPage = () => {
  const [form] = Form.useForm();
  const { userName } = useContext(GlobalContext) || {};
  const isAdminUser = !!userName && isAdmin(userName);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    adminService.getSharing().then(result => {
      if (result.response.ok && result.apiData?.data) {
        form.setFieldValue('data', result.apiData?.data);
      } else {
        showErrorMessage(joinMessages(result.apiData?.messages))
      }
    });
  }, [form]);

  const handleFinish = ({ data }: { data: string }) => {
    console.log(data);
    setLoading(true);
    adminService.pushSharing(data).then(result => {
      if (result.response.ok) {
        showSuccessMessage('Succesfully pushed data!');
      } else {
        showErrorMessage(joinMessages(result.apiData?.messages))
      }
      setLoading(false);
    });
  }

  return (
    <Card title={
      <Row align="middle">
        <Col flex="none">Data sharing</Col>
      </Row>
    } bordered={false}>
      <AppSpin spinning={false}>
        <Form
          layout="vertical"
          form={form}
          onFinish={handleFinish}
          onFinishFailed={(err) => console.log(err)}
          autoComplete="off"
        >
          <Row gutter={[16, 16]}>
            <Col xs={24}>
              <Form.Item label="Data" name="data">
                <Input.TextArea readOnly={!isAdminUser} style={{ height: 350 }} />
              </Form.Item>
            </Col>
          </Row>
          {
            isAdminUser &&
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
          }
        </Form>
      </AppSpin>
    </Card>
  )
};

export default SharingPage;
