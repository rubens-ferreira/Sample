import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";

const API_URL = "http://localhost:5001/api/users";

export const loadUsers = createAsyncThunk("user/loadUsers", async () => {
  const response = await fetch(API_URL);
  if (!response.ok) {
    const json = await response.json();
    throw new Error("Failed to load users: " + json.error);
  }
  const data = await response.json();
  return data;
});

export const loadUser = createAsyncThunk("user/loadUser", async (id) => {
  const response = await fetch(API_URL + `/${id}`);
  if (!response.ok) {
    const json = await response.json();
    throw new Error("Failed to load users: " + json.error);
  }
  const data = await response.json();
  return data;
});

export const loadUserDetails = createAsyncThunk(
  "user/loadUserDetails",
  async (id) => {
    const response = await fetch(API_URL + `/details/${id}`);
    if (!response.ok) {
      const json = await response.json();
      throw new Error("Failed to load users: " + json.error);
    }
    const data = await response.json();
    return data;
  }
);

export const saveUser = createAsyncThunk(
  "user/saveUser",
  async (userToSave) => {
    const method = userToSave.userID ? "PUT" : "POST";
    const response = await fetch(API_URL, {
      method: method,
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(userToSave),
    });

    if (!response.ok) {
      const json = await response.json();
      throw new Error("Failed to save user: " + json.error);
    }

    const savedUserData = userToSave.userID
      ? userToSave
      : await response.json();
    return savedUserData;
  }
);

const initialState = {
  loading: false,
  users: [],
  user: null,
  message: "",
  userID: "",
};

export const userSlice = createSlice({
  name: "user",
  initialState,
  reducers: {
    dismissMessage: (state) => {
      state.message = "";
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(loadUsers.pending, (state) => {
        state.loading = true;
        state.user = null;
      })
      .addCase(loadUsers.fulfilled, (state, action) => {
        state.loading = false;
        state.users = action.payload;
      })
      .addCase(loadUsers.rejected, (state, action) => {
        state.loading = false;
        state.message = action.error.message ?? "";
      })
      .addCase(loadUser.pending, (state) => {
        state.loading = true;
      })
      .addCase(loadUser.fulfilled, (state, action) => {
        state.loading = false;
        state.user = action.payload;
      })
      .addCase(loadUser.rejected, (state, action) => {
        state.loading = false;
        state.message = action.error.message ?? "";
      })
      .addCase(loadUserDetails.pending, (state) => {
        state.loading = true;
        state.user = null;
      })
      .addCase(loadUserDetails.fulfilled, (state, action) => {
        state.loading = false;
        state.user = action.payload;
      })
      .addCase(loadUserDetails.rejected, (state, action) => {
        state.loading = false;
        state.message = action.error.message ?? "";
      })
      .addCase(saveUser.pending, (state) => {
        state.loading = true;
      })
      .addCase(saveUser.fulfilled, (state, action) => {
        const savedUser = action.payload;
        state.loading = false;
        const existingUserIndex = state.users.findIndex(
          (user) => user.userID === savedUser.userID
        );
        if (existingUserIndex !== -1) {
          state.users[existingUserIndex] = savedUser;
        } else {
          state.users.push(savedUser);
        }
        state.user = null;
      })
      .addCase(saveUser.rejected, (state, action) => {
        state.loading = false;
        state.message = action.error.message ?? "";
      });
  },
});

export const { dismissMessage } = userSlice.actions;

export const selectLoading = (state) => state.user.loading;
export const selectUsers = (state) => state.user.users;
export const selectMessage = (state) => state.user.message;
export const selectUser = (state) => state.user.user;
