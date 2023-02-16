import { Button, ButtonProps } from 'antd';
import React from 'react';

interface Props extends ButtonProps {
}

const AppCardHeaderButton: React.FC<Props> = ({ style, ...props }) => {
  return <Button style={{
    lineHeight: 0,
    height: 22,
    ...style
  }} {...props} />
};

export default AppCardHeaderButton;
