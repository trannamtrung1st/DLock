import { antdNotification } from "@shared/components/antd-functions";
import { Typography } from "antd";
import { ArgsProps } from "antd/es/notification/interface";

const { Text } = Typography;

const showNotification = ({
  description, key, ...args }: ArgsProps) => {
  key = key || new Date().getTime().toString();
  antdNotification.open({
    ...args, key,
    description: typeof description === 'string'
      ? <Text className="dl-pre-wrap">{description}</Text>
      : description,
  });
  return key;
};

const showInfoNotification = (args: {
  message: ArgsProps['message'],
  description: ArgsProps['description'],
  duration?: ArgsProps['duration'],
  icon?: ArgsProps['icon'],
  onClick?: ArgsProps['onClick'],
  style?: ArgsProps['style'],
  key?: ArgsProps['key'],
  placement?: ArgsProps['placement']
}) => {
  return showNotification({
    ...args,
    type: 'info',
  });
};

const showWarningNotification = (args: {
  message: ArgsProps['message'],
  description: ArgsProps['description'],
  duration?: ArgsProps['duration'],
  onClick?: ArgsProps['onClick'],
  style?: ArgsProps['style'],
  key?: ArgsProps['key'],
  placement?: ArgsProps['placement']
}) => {
  return showNotification({
    ...args,
    type: 'warning'
  })
};

const closeNotification = (key: ArgsProps['key']) => {
  antdNotification.destroy(key);
}

export {
  showInfoNotification,
  showWarningNotification,
  closeNotification
}