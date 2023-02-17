import { scrollTo } from "@shared/helpers/display.helper";
import { useEffect } from "react";
import { useLocation } from "react-router-dom";

const ScrollToTop = () => {
  const { pathname } = useLocation();

  useEffect(() => {
    scrollTo({ x: 0, y: 0 });
  }, [pathname]);

  return <></>;
}

export default ScrollToTop;