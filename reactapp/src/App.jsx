import React, {Component} from 'react';
import ToDoScreen from "@/Screens/ToDo/index.jsx";
import LoginScreen from "@/Screens/Auth/Login.jsx";
import {AppRouter} from "@/Routes/AppRouter.jsx";

export default class App extends Component {
    static displayName = "Demo ToDo App";

    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>
                <AppRouter
                    {...this.props}
                />
            </div>
        );
    }
}
