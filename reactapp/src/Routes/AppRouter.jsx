import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import {useSelector} from "react-redux";
import {PrivateRoutes} from "@/Routes/PrivateRoutes.jsx";
import {PublicRoutes} from "@/Routes/PublicRoutes.jsx";
import {useEffect} from "react";

export const AppRouter = () => {

    const appSlicer = useSelector(state => state.metamaskRdx);

    useEffect(() => {
        console.log("appSlicer.isLoggedIn", appSlicer);
    }, []);

    return (
        <BrowserRouter>
            <Routes>
                {
                    appSlicer.isLoggedIn && appSlicer.userToken
                        ? <Route path="/*" element={<PrivateRoutes />} />
                        : <Route path="/*" element={<PublicRoutes />} />
                }

                <Route path='*' element={<Navigate to='/login' replace />} />
            </Routes>
        </BrowserRouter>
    )
}