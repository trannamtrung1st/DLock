import { routeMap } from '@shared/constants/routes.const';
import { Button, Result } from 'antd';
import React from 'react';
import { Link } from 'react-router-dom';

const AccessDeniedPage = () => {
  return (
    <Result
      status="403"
      title="403"
      subTitle="Không có quyền truy cập."
      extra={(
        <Link to={routeMap.root.link}>
          <Button type="primary">Về trang chủ</Button>
        </Link>
      )}
    />
  )
};

export default AccessDeniedPage;
