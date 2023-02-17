import { GlobalContext } from '@app/contexts/global.context';
import { identityService } from '@app/services';
import { joinMessages, showErrorMessage } from '@shared/helpers/messages.helper';
import { Button, Divider, Form, Input, Typography } from 'antd';
import React, { useContext, useState } from 'react';

const { Title, Text, Paragraph } = Typography;

const LoginPage = () => {
  const { setUserName } = useContext(GlobalContext) || {};
  const [errorMessages,] = useState<string[]>([]);
  const [loading,] = useState(false);

  const onFinish = (values: { userName: string }) => {
    if (!setUserName) return;
    const { userName } = values;
    console.log(values);
    identityService.login(userName).then((resp) => {
      if (resp.response.ok) {
        identityService.saveUserName(userName);
        setUserName(userName);
      } else {
        showErrorMessage(joinMessages(resp.apiData?.messages));
      }
    });
  };

  const onFinishFailed = (errorInfo: any) => {
    console.log('Failed:', errorInfo);
  };

  return (
    <div style={{
      height: '100vh',
      display: 'flex',
      flexDirection: 'column',
      justifyContent: 'center',
      alignItems: 'center',
    }}>
      <Title style={{ textAlign: 'center' }}>DLock</Title>
      <Form
        style={{
          width: 380,
          maxWidth: '88%',
          background: '#e0f1ff',
          borderRadius: 8,
          border: '2px solid #bae0ff',
          padding: 24
        }}
        layout="vertical"
        initialValues={{}}
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
      >
        <Divider children={'ĐĂNG NHẬP'} />

        <Form.Item
          label="Tài khoản"
          name="userName"
          rules={[{ required: true, whitespace: true, message: 'Vui lòng nhập tài khoản!' }]}
        >
          <Input />
        </Form.Item>

        <Form.Item style={{ textAlign: 'center' }}>
          <Paragraph>
            {errorMessages?.map((mess, idx) => <div key={idx}><Text type="danger">{mess}</Text></div>)}
          </Paragraph>
          <Button style={{ width: '60%' }} loading={loading} type="primary" htmlType="submit">
            Đăng nhập
          </Button>
        </Form.Item>
      </Form>
    </div>
  );
};

export default LoginPage;
