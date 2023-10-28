import React, {useEffect, useMemo, useState} from "react";
import {useSelector} from "react-redux";
import axios from "axios";
import {toast, ToastContainer} from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';
import HeaderComp from "@/Components/Header.jsx";

const ToDoScreen = (props) => {

    const appSlicer = useSelector(state => state.metamaskRdx);
    
    const [loading, setLoading] = useState(false);
    const [tasks, setTasks] = useState([]);

    const populateTasks = () => {
        setLoading(true);
        return new Promise((resolve, reject) => {
            axios.get("/v1/Tareas", {
                headers: {
                    'Authorization': 'Bearer ' + appSlicer.userToken,
                    'Content-Type': 'application/json'
                }
            })
                .then(response => {
                    console.log(response);
                    toast("🦄 Task obtained successfully");
                    resolve(response);
                })
                .catch(error => {
                    console.log(error);
                    toast.info("Error: " + error?.response?.statusText ?? "" + " " + error?.response?.status)
                    reject(error);
                })
                .finally(() => {
                    setLoading(false);
                });
        });
    }

    useEffect(() => {
        populateTasks()
            .then(response => {
                setTasks(response.data);
            })
            .catch(error => {
                setTasks([]);
            });
    }, []);
    
    return (
        <div
            className={'flex flex-col items-center justify-start h-screen w-screen'}
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
            
            <HeaderComp
                title={'ToDo Demo App'}
            />
        </div>
    );
};

export default ToDoScreen;