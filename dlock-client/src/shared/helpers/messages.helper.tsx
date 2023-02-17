import { antdMessage } from "@shared/components/antd-functions";
import { appMessages } from "@shared/constants/messages.const";
import { sleep } from "@shared/helpers/common.helper";
import { Typography } from "antd";
import { NoticeType } from 'antd/es/message/interface';

const { Text } = Typography;

const errorMessageDefault = (message?: string) => (message?.length && message) || appMessages.common.unknownError;
const successMessageDefault = (message?: string) => (message?.length && message) || appMessages.common.success;

const showMessage = async (message: string, type: NoticeType, delayMs: number = 0) => {
  if (delayMs > 0) await sleep(delayMs);
  antdMessage.open({
    content: typeof message === 'string'
      ? <Text className="dl-pre-wrap">{message}</Text>
      : message,
    type
  })
};

const showWarningMessage = async (message: string, delayMs: number = 0) => {
  await showMessage(message, 'warning', delayMs);
};

const showSuccessMessage = async (message?: string, delayMs: number = 0) => {
  message = successMessageDefault(message);
  await showMessage(message, 'success', delayMs);
};

const showErrorMessage = async (message?: string, delayMs: number = 0) => {
  message = errorMessageDefault(message);
  await showMessage(message, 'error', delayMs);
}

const joinMessages = (messages?: string[], separator = '\n') =>
  messages?.join(separator);

export {
  showErrorMessage,
  showSuccessMessage,
  showWarningMessage,
  showMessage,
  errorMessageDefault,
  successMessageDefault,
  joinMessages
}