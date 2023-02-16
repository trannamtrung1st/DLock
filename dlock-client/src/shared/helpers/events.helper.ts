const onKeyDownCb = (callbackByKey: {
  [key: string]: ((ev: React.KeyboardEvent) => void) | undefined
}) => (ev: React.KeyboardEvent) => {
  const handler = callbackByKey[ev.key];
  if (handler) {
    handler(ev);
  }
};

const { addBeforeUnloadListener, removeBeforeUnloadListener } = (() => {
  let lastHandler: any = undefined;
  return {
    addBeforeUnloadListener: (handler: (event: BeforeUnloadEvent) => void) => {
      removeBeforeUnloadListener();
      lastHandler = handler;
      window.addEventListener("beforeunload", handler, { capture: true });
    },
    removeBeforeUnloadListener: () => {
      if (lastHandler) {
        window.removeEventListener("beforeunload", lastHandler, { capture: true });
      }
    }
  }
})();

export {
  onKeyDownCb,
  addBeforeUnloadListener,
  removeBeforeUnloadListener
}