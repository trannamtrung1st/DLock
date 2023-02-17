import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.scss';
import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";
import { App as AntdApp, ConfigProvider } from 'antd';
import AppGlobalHandler from '@shared/components/app-global-handler';
import routerModules from '@shared/configs/router-modules.config';
import AntdFunctions from '@shared/components/antd-functions';

window.onerror = (e, s, l, c, err) => {
  // [IMPORTANT] catch Antd tabs error
  if (err?.stack && /.*(querySelector).*(callRef).*/s.test(err.stack)) {
    console.log('Caught Antd tabs error.');
    return true;
  }
};

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

const router = createBrowserRouter(routerModules);

root.render(
  <ConfigProvider
    theme={{
      components: {
        Layout: {
          colorBgHeader: '#FFF'
        }
      },
      token: {
      }
    }}
  >
    <AntdApp>
      <AntdFunctions />
      <AppGlobalHandler>
        <RouterProvider router={router} />
      </AppGlobalHandler>
    </AntdApp>
  </ConfigProvider>
);