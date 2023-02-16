const getPlaceholderDom = (entityId: string, type: string) => {
  const domQuery = `[data-${type}-id="${entityId}"] [data-rbd-placeholder-context-id]`;
  const dom = document.querySelector(domQuery) as HTMLDivElement;
  return dom;
};

export {
  getPlaceholderDom
}