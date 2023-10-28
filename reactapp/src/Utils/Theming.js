import {createTheme} from "@mui/material";
import {appColors} from "./AppColors";

export const themeApp = createTheme({
    mode: window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light',
    typography: {
        fontFamily: [
            'Poppins',
        ].join(','),
    },
    palette: {
        primary: {
            main: appColors.primary,
            contrastText: appColors.white,
        },
        secondary: {
            main: appColors.secondary,
        },
        success: {
            main: appColors.success,
            dark: appColors.successDark,
            light: appColors.successLight,
            contrastText: appColors.white,
        },
        info: {
            main: appColors.info,
            dark: appColors.infoDark,
            light: appColors.infoLight,
        },
        warning: {
            main: appColors.warning,
            dark: appColors.warningDark,
            light: appColors.warningLight,
        },
        error: {
            main: appColors.error,
            dark: appColors.errorDark,
            light: appColors.errorLight,
        }
    }
});
