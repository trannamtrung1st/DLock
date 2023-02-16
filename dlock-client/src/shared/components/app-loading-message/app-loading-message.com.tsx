import { Card, Space } from 'antd';
import React from 'react';
import AppSpin from '../app-spin';

interface Props {
  loading: boolean;
  message?: string;
}

const AppLoadingMessage: React.FC<Props> = ({
  loading, message
}) => {
  return loading ? <Card style={{
    position: 'fixed', bottom: 16, right: 16
  }} bodyStyle={{
    padding: '8px 16px', margin: 0
  }}>
    <Space>
      <AppSpin fontSize={18} spinning={true} />
      {message || 'Đang thực hiện ...'}
    </Space>
  </Card> : null;
};

export default AppLoadingMessage;
