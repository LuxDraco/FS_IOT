import { Navigate, Route, Routes } from 'react-router-dom';
import LoginScreen from "@/Screens/Auth/Login.jsx";

export const PublicRoutes = () => {
    return (
        <Routes>
            <Route path='login' element={<LoginScreen />} />
            <Route path='*' element={<Navigate to='/login' replace />} />
        </Routes>
    );
};