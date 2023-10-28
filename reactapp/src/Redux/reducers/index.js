import {combineReducers} from "@reduxjs/toolkit";
import appSlicer from "@/Redux/reducers/app-slice.js";

const rootReducer = combineReducers({
    metamaskRdx: appSlicer,
});

export default rootReducer;