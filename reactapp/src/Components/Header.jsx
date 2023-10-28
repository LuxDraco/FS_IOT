import React from "react";
import {AppBar, Button, Toolbar, Typography} from "@mui/material";
import IconHeader from "@/assets/check.png";

const HeaderComp = (props) => {
    return (
        <AppBar
            position="static"
            color="transparent"
            elevation={2}
        >
            <Toolbar>
                <div 
                    className={'flex justify-start items-center w-[500px] bg-amber-500'}
                >
                    <img
                        src={IconHeader}
                        alt="Logo"
                        style={{
                            width: "40px",
                            height: "40px",
                            marginRight: "10px",
                        }}
                    />

                    <Typography
                        variant="h6"
                        noWrap
                        component="div"
                        sx={{ display: { xs: 'none', sm: 'block' } }}
                    >
                        {props.title}
                    </Typography>

                    <div
                        className={'flex justify-end items-center flex-1 flex-grow bg-blue-600'}
                    >
                        <Button>
                            LogOut
                        </Button>
                    </div>
                </div>
            </Toolbar>
        </AppBar>
    )
}

export default HeaderComp;