import AppLayout from "@shared/components/app-layout";
import NotFoundPage from "@shared/pages/not-found";
import React from "react";
import { Navigate, Outlet, RouteObject } from "react-router";
import AppAnonymous from "@shared/components/app-anonymous";
import ScrollToTop from "@shared/components/scroll-to-top";
import AppProtected from "@shared/components/app-protected";
import RouterFunctions from "@shared/components/router-functions";
import LoginPage from "@app/pages/login";
import HomePage from "@app/pages/home";
import BookingPage from "@app/pages/booking";
import AdminPage from "@app/pages/admin";
import SharingPage from "@app/pages/sharing";

const mainModule: RouteObject = {
  element: (
    <AppProtected>
      <Outlet />
    </AppProtected>
  ),
  children: [
    {
      path: '/',
      element: <AppLayout />,
      errorElement: null,
      children: [
        {
          path: '',
          element: <Navigate to={'home'} />
        },
        {
          path: 'home',
          element: <HomePage />
        },
        {
          path: 'booking',
          element: <BookingPage />
        },
        {
          path: 'sharing',
          element: <SharingPage />
        },
        {
          path: 'admin',
          element: <AdminPage />
        },
        {
          path: '*',
          element: <NotFoundPage />
        }
      ]
    }
  ]
};

const publicModule: RouteObject = {
  element: (
    <AppAnonymous>
      <Outlet />
    </AppAnonymous>
  ),
  children: [
    {
      path: 'login',
      element: <LoginPage />
    },
    {
      path: '*',
      element: <NotFoundPage />
    }
  ]
};

const routerModules: RouteObject[] = [
  {
    element: (
      <>
        <RouterFunctions />
        <ScrollToTop />
        <Outlet />
      </>
    ),
    children: [
      mainModule,
      publicModule
    ]
  }
];

export default routerModules;
