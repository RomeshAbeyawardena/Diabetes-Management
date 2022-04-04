export const DialogTypes = {
    DatePicker: "date-picker",
    Login: "login",
    Register: "register",
    TextEntry: "text-entry",
    NumberPicker: "number-picker"
};

export function getTitle(component) {
    switch(component){
        case DialogTypes.DatePicker:
            return "Pick a date/time";
        case DialogTypes.Login:
            return "Sign in";
        case DialogTypes.NumberPicker:
            return "Pick a unit value";
        case DialogTypes.Register:
            return "Sign up"
        case DialogTypes.TextEntry:
            return "Description";
    }
    return "";
}