import { appMessages } from "@shared/constants/messages.const";

const handledErrorMessage = '__handled__';

const handledError = new Error(handledErrorMessage);

const isHandledError = (e: any) => e === handledError || e.message === handledErrorMessage;

const throwUnknownError = () => {
  throw new Error(appMessages.common.unknownError);
};

export {
  handledError,
  isHandledError,
  throwUnknownError
}