import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { Register } from 'src/app/models/register';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.sass']
})
export class RegisterComponent implements OnInit {
    
    form: FormGroup;
    
    constructor(
        private fb: FormBuilder,
        private authService: AuthService,
        private toastr: ToastrService,
        private router: Router)
        {
            this.form = this.fb.group({
                username: ['test-user', Validators.required],
                email: ['test@test.test', Validators.required],
                character: ['test-user', Validators.required],
                server : ['Meriana', Validators.required],
                password: ['testtest', Validators.required],
                passwordConfirm: ['testtest', Validators.required],
                subscribe : [false, Validators.required]
            });
        }
        
    ngOnInit()
    {
    }

    public async register()
    {
        const val = this.form.value;

        console.log(val.username);
        console.log(val.email);
        console.log(val.character);
        console.log(val.server);
        console.log(val.password);
        console.log(val.passwordRepeat);
        console.log(val.subscribe);

        if (this.form.valid)
        {
            const register = new Register();
            register.username = this.form.value.username;
            register.email = this.form.value.email;
            register.character = this.form.value.character;
            register.server = this.form.value.server;
            register.password = this.form.value.password;
            register.passwordConfirm = this.form.value.passwordConfirm;
            register.subscribe = this.form.value.subscribe;

            const result = await this.authService.register(register);

            if (result)
            {
                if (result.wasCreated)
                {
                    this.toastr.success('Account successfully created', 'Success');
                    this.router.navigateByUrl('/login');
                }
                else
                {
                    if (result.usernameTaken)
                    {
                        this.toastr.warning('Username already taken', 'Conflict');
                    }

                    if (result.emailTaken)
                    {
                        this.toastr.warning('Email already taken', 'Conflict');
                    }
                }
            }
            else
            {
                this.toastr.error('An error occured', 'Error');
            }
        }
    }
}
    