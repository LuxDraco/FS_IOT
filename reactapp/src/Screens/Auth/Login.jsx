import React, {useState} from "react";
import {useDispatch, useSelector} from "react-redux";
import axios from "axios";
import {Button, IconButton, Paper, TextField, Typography} from "@mui/material";
import {VisibilityOutlined, VisibilityOffOutlined} from '@mui/icons-material';
import {toast, ToastContainer} from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';
import {setIsLogged, setUserToken} from "@/Redux/reducers/app-slice.js";

const LoginScreen = (props) => {

    const appSlicer = useSelector(state => state.metamaskRdx.value); //P@ss.W0rd
    const dispatch = useDispatch();

    const [loading, setLoading] = useState(false);
    const [showPassword, setShowPassword] = useState(false);

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    const submitLogin = () => {
        setLoading(true);

        const formData = {
            userName: username,
            password: password,
        };

        return new Promise((resolve, reject) => {
            axios.post("/token", formData)
                .then(response => {
                    console.log(response);
                    toast.success("Login Success");
                    resolve(response);
                })
                .catch(error => {
                    console.log(error);
                    toast.info("Login Failed: " + error?.response?.statusText ?? "" + " " + error?.response?.status)
                    reject(error);
                })
                .finally(() => {
                    setLoading(false);
                });
        });

    }

    return (
        <div
            className={'flex flex-col items-center justify-center h-screen'}
        >
            <ToastContainer
                position="bottom-right"
                autoClose={5000}
                draggable
                pauseOnHover
                pauseOnFocusLoss
                hideProgressBar={false}
                theme={'light'}
                style={{
                    zIndex: 99999999,
                }}
            />
            
            <Paper
                elevation={3}
                className={''}
                style={{
                    width: "100%",
                    maxWidth: "500px",
                    padding: "20px",
                }}
            >
                <Typography
                    variant="h5"
                    className={'!font-semibold tracking-wide text-center !mb-5'}
                >
                    Login
                </Typography>

                <TextField
                    id="outlined-basic"
                    label="Username"
                    fullWidth
                    required
                    variant="outlined"
                    value={username}
                    onChange={(e) => {
                        setUsername(e.target.value);
                    }}
                />

                <TextField
                    id="outlined-basic"
                    label="Password"
                    type={showPassword ? "text" : "password"}
                    className={'!mt-5 !mb-10'}
                    fullWidth
                    required
                    variant="outlined"
                    value={password}
                    onChange={(e) => {
                        setPassword(e.target.value);
                    }}
                    InputProps={{
                        endAdornment: (
                            <IconButton
                                color={'secondary'}
                                onClick={() => {
                                    setShowPassword(!showPassword);
                                }}
                            >
                                {showPassword ?
                                    <VisibilityOutlined/> :
                                    <VisibilityOffOutlined/>
                                }
                            </IconButton>
                        )
                    }}
                />

                <Button
                    variant="contained"
                    fullWidth
                    disabled={loading || username === "" || password === ""}
                    onClick={() => {
                        submitLogin()
                            .then(response => {
                                dispatch(setUserToken(response.data['accessToken']));
                                dispatch(setIsLogged(true));
                            })
                            .catch(error => {
                                console.log(error);
                                dispatch(setUserToken(null));
                                dispatch(setIsLogged(false));
                            });
                    }}
                >
                    Login
                </Button>
            </Paper>
        </div>
    );
}

export default LoginScreen;