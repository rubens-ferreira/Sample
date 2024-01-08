import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";

const API_URL = "http://localhost:5001/api/accounts/currentaccount";

export const createNewCurrentAccount = createAsyncThunk(
  "account/createNewCurrentAccount",
  async (currentAccount) => {
    const response = await fetch(API_URL, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(currentAccount),
    });
    if (!response.ok) {
      const json = await response.json();
      throw new Error("Failed to add current account: " + json.error);
    }
  }
);

const initialState = {
  saving: false,
  message: "",
};

export const accountSlice = createSlice({
  name: "account",
  initialState,
  reducers: {
    dismissMessage: (state) => {
      state.message = "";
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(createNewCurrentAccount.pending, (state) => {
        state.saving = true;
      })
      .addCase(createNewCurrentAccount.fulfilled, (state) => {
        state.saving = false;
      })
      .addCase(createNewCurrentAccount.rejected, (state, action) => {
        state.saving = false;
        state.message = action.error.message ?? "";
      });
  },
});

export const { dismissMessage } = accountSlice.actions;

export const selectSaving = (state) => state.account.saving;
export const selectMessage = (state) => state.account.message;
