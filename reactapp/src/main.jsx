import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App.jsx';
import { ThemeProvider } from "@mui/material";
import './index.css';

import { Provider } from 'react-redux';
import { PersistGate } from 'redux-persist/integration/react';
import {themeApp} from "@/Utils/Theming.js";
import {persistor, store} from "@/Redux/store/index.js";

ReactDOM.createRoot(document.getElementById('root')).render(
    <React.StrictMode>
        <Provider store={store}>
            <PersistGate loading={null} persistor={persistor}>
                <ThemeProvider theme={themeApp}>
                    <App />
                </ThemeProvider>
            </PersistGate>
        </Provider>
    </React.StrictMode>,
)
