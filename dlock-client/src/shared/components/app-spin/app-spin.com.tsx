import { Spin, SpinProps } from 'antd';
import React from 'react';
import {
  LoadingOutlined
} from '@ant-design/icons';

const defaultFontSize = 24;

interface Props extends SpinProps {
  fontSize?: number;
}

export const appSpinProps: SpinProps = {
  indicator: (
    <LoadingOutlined style={{ fontSize: defaultFontSize }} spin />
  )
};

const AppSpin: React.FC<Props> = ({ fontSize, ...props }) => {
  return <Spin
    {...appSpinProps}
    indicator={<LoadingOutlined style={{ fontSize: fontSize || defaultFontSize }} spin />}
    {...props} />
};

export default AppSpin;
