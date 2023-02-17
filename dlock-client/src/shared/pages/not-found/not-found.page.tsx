import { routeMap } from '@shared/constants/routes.const';
import { Button, Result } from 'antd';
import React from 'react';
import { Link } from 'react-router-dom';

const NotFoundPage = () => {
  return (
    <Result
      status="404"
      title="404"
      subTitle="Xin lỗi, trang bạn tìm kiếm không tồn tại"
      extra={(
        <Link to={routeMap.root.link}>
          <Button type="primary">Về trang chủ</Button>
        </Link>
      )}
    />
  )
};

export default NotFoundPage;
