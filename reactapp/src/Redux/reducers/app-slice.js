import {createSlice} from '@reduxjs/toolkit';

const initialState = {
    title: 'Demo ToDo IoT',
    version: '1.0.0',
    userToken: null,
    isLoggedIn: false
};

const appSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        setTitle: (state, action) => {
            state.title = action.payload;
        },
        setVersion: (state, action) => {
            state.version = action.payload;
        },
        setUserToken: (state, action) => {
            state.userToken = action.payload;
        },
        setIsLogged: (state, action) => {
            state.isLoggedIn = action.payload;
        }
    }
});

export const {setTitle, setVersion, setUserToken, setIsLogged} = appSlice.actions;
export default appSlice.reducer;