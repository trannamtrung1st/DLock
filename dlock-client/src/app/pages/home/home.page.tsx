import { Card, Col, Row, Typography } from 'antd';
import React from 'react';
import AppSpin from '@shared/components/app-spin';

const { Paragraph } = Typography;

const HomePage = () => {

  return (
    <Card title={
      <Row align="middle">
        <Col flex="none">Welcome to DLock</Col>
      </Row>
    } bordered={false}>
      <AppSpin spinning={false}>
        <Paragraph style={{ margin: 0 }}>
          Let's try DLock
        </Paragraph>
      </AppSpin>
    </Card>
  )
};

export default HomePage;
