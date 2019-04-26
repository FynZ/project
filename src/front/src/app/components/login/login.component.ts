import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Login } from 'src/app/models/login';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.sass']
})
export class LoginComponent implements OnInit
{
    form: FormGroup;

    constructor(
        private fb: FormBuilder,
        private authService: AuthService,
        private toastr: ToastrService,
        private router: Router)
    {
        this.form = this.fb.group({
            email: ['test@test.test', Validators.required],
            password: ['testtest', Validators.required],
            rememberMe : [false, Validators.required]
        });
    }

    async login()
    {
        if (this.form.valid)
        {
            const login = new Login();
            login.email = this.form.value.email;
            login.password = this.form.value.password;
            login.rememberMe = this.form.value.rememberMe; 

            if (await this.authService.login(login))
            {
                this.toastr.success('Successfully logged in', 'Success');
                this.router.navigateByUrl('/monsters');
            }
            else
            {
                this.toastr.error('Email or password is incorrect', 'Error');
            }
        }
    }

    ngOnInit()
    {
    }

}
