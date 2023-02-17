import { dateTimeFormat } from "@shared/constants/formatting.const";
import moment from "moment";

const language = window.navigator.language || (window.navigator.languages && window.navigator.languages[0]);
const numberFormatter = new Intl.NumberFormat(language);

const formatClientDate = (standardDateStr?: moment.MomentInput) =>
  !!standardDateStr ? moment(standardDateStr).format(dateTimeFormat.clientDate) : undefined;

const formatClientDateTime = (standardDateStr?: moment.MomentInput) =>
  !!standardDateStr ? moment(standardDateStr).format(dateTimeFormat.clientDateTimeHHmmss) : undefined;

const formatNumber = (value: number) => {
  return numberFormatter.format(value);
}

export {
  formatNumber,
  formatClientDate,
  formatClientDateTime
}