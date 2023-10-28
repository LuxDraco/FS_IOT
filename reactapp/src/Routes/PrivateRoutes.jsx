import { Navigate, Route, Routes } from 'react-router-dom';
import ToDoScreen from "@/Screens/ToDo/index.jsx";

export const PrivateRoutes = () => {
    return (
        <Routes>
            <Route path='/' element={<ToDoScreen />} />
            <Route path='*' element={<Navigate to='/' replace />} />
        </Routes>
    );
};