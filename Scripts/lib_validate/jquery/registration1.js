
           /* <![CDATA[ */
           jQuery(function(){
                
                
               
                
               jQuery("#ValidEmail").validate({
                   expression: "if (VAL.match(/^[^\\W][a-zA-Z0-9\\_\\-\\.]+([a-zA-Z0-9\\_\\-\\.]+)*\\@[a-zA-Z0-9_]+(\\.[a-zA-Z0-9_]+)*\\.[a-zA-Z]{2,4}$/)) return true; else return false;",
                   message: "Please enter a valid email and ensure there are no spaces before or after the email"
               });
               jQuery("#ValidConfirmEmail").validate({
                   expression: "if ((VAL == jQuery('#ValidEmail').val()) && VAL) return true; else return false;",
				   message: "Emails do not match. If emails are the same check for spaces before and after email"
               });
               // jQuery("#ValidPassword").validate({
               //   expression: "if (VAL.length > 7 && VAL) return true; else return false;",
               // message: "Please enter a valid Password"
               //});
               jQuery("#ValidPassword").validate({
                   expression: "if (VAL.match(/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/)) return true; else return false;",
                   message: "Password Must Contain 8 Characters, an uppercase letter, a number and special character"
               });
               
               jQuery("#ValidConfirmPassword").validate({
                   expression: "if ((VAL == jQuery('#ValidPassword').val()) && VAL) return true; else return false;",
                   message: "Confirm password doesn't match the chosen password "
               });
                
                
                
               
				
           });
/* ]]> */

