//---- Variables ----//

var Validator;

(function (Validator) {
    // Regular Expressions
    var alphanumericRegexp = /^[A-Z a-z 0-9]+$/;
    var lettersRegexp =  /^[A-Z a-z]+$/;
    var numberRegexp = /^[0-9.0-9]+$/;
    var timeRegexp = /\b((1[0-2]|0?[1-9]):([0-5][0-9]) ([AaPp][Mm]))/;
    var dateRegexp = /^(?:(0[1-9]|1[012])[\/.](0[1-9]|[12][0-9]|3[01])[\/.](19|20)[0-9]{2})$/;
    var HourRegexp = /^\d{0,2}(\.\d{1,2})?$/;
    //var HourRegexp = /^\d+(\.\d{1,2})?$/;   
    var TimeRegexp = /^(([0-2]?[0-9])|([2][0-3])):([0-5]?[0-9])?$/i;

    // Validator to check for Required value
    var RequiredValidator = (function () {
        function RequiredValidator() {
        }
        RequiredValidator.prototype.isAcceptable = function (input) {
            if (input == undefined || input.trim() == "")
                return false;
            else
                return true;
        };
        return RequiredValidator;
    }());
    Validator.RequiredValidator = RequiredValidator;

    // Validator to check for Alphanumeric value
    var AlphanumericValidator = (function () {
        function AlphanumericValidator() {
        }
        AlphanumericValidator.prototype.isAcceptable = function (input) {
            return alphanumericRegexp.test(input);
        };
        return AlphanumericValidator;
    }());
    Validator.AlphanumericValidator = AlphanumericValidator;

    // Validator to check for Numeric value
    var NumberValidator = (function () {
        function NumberValidator() {
        }
        NumberValidator.prototype.isAcceptable = function (input) {
            return numberRegexp.test(input);
        };
        return NumberValidator;
    }());
    Validator.NumberValidator = NumberValidator;

    // Validator to check for Time value in AM/PM
    var TimeValidator = (function () {
        function TimeValidator() {
        }
        TimeValidator.prototype.isAcceptable = function (input) {
            return timeRegexp.test(input);
        };
        return TimeValidator;
    }());
    Validator.TimeValidator = TimeValidator;

    // Validator to check for Date in MM/DD/YYYY
    var DateValidator = (function () {
        function DateValidator() {
        }
        DateValidator.prototype.isAcceptable = function (input) {
            return dateRegexp.test(input);
        };
        return DateValidator;
    }());
    Validator.DateValidator = DateValidator;

    // Validator to check for Hours in 00.00
    var HourValidator = (function () {
        function HourValidator() {
        }
        HourValidator.prototype.isAcceptable = function (input) {
            return HourRegexp.test(input);
        };
        return HourValidator;
    }());
    Validator.HourValidator = HourValidator;

    // Validator to check for Time in 24 hour format 24:60
    var TimeValidator = (function () {
        function TimeValidator() {
        }
        TimeValidator.prototype.isAcceptable = function (input) {
            input = input.replace("am", "").replace("AM", "").replace("pm", "").replace("PM", "").trim();
            var time = input.split(":");
            if (time.length == "2")
            {
                if (numberRegexp.test(time[0]) && numberRegexp.test(time[1])) {
                    if (time[0] > 23 || time[1] > 59)
                        return false;
                    else
                        return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        };
        return TimeValidator;
    }());
    Validator.TimeValidator = TimeValidator;

})(Validator || (Validator = {}));