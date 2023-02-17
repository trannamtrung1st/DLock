import { Card, Col, Row, Typography } from 'antd';
import React, { CSSProperties } from 'react';
import AppSpin from '@shared/components/app-spin';

const { Paragraph } = Typography;

const homeCardStyle: CSSProperties = {
  marginBottom: 16
};

const HomePage = () => {

  return (
    <Card style={homeCardStyle} title={
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
