import { App, theme } from 'antd';
import { MessageInstance } from "antd/es/message/interface";
import { ModalStaticFunctions } from "antd/es/modal/confirm";
import { NotificationInstance } from "antd/es/notification/interface";
import { GlobalToken } from 'antd/es/theme/interface';

let antdMessage: MessageInstance;
let antdNotification: NotificationInstance;
let antdModal: Omit<ModalStaticFunctions, 'warn'>;
let antdGlobalToken: GlobalToken;

const AntdFunctions = () => {
  const { message, modal, notification } = App.useApp();
  const { token } = theme.useToken();
  antdGlobalToken = token;
  antdMessage = message;
  antdModal = modal;
  antdNotification = notification;
  return null;
};

export default AntdFunctions;

export { antdMessage, antdNotification, antdModal, antdGlobalToken };