import { configureStore } from "@reduxjs/toolkit";
import { userSlice } from "./User/userSlice";
import { accountSlice } from "./Account/accountSlice";

export const store = configureStore({
  reducer: {
    user: userSlice.reducer,
    account: accountSlice.reducer,
  },
});
