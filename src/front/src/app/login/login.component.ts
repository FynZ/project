import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { LoginResult } from '../models/login-result';
import { HttpEvent } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

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
        const val = this.form.value;

        if (val.email && val.password)
        {
            if (await this.authService.login(val.email, val.password))
            {
                this.toastr.success('Successfully logged in', 'Success');
                this.router.navigateByUrl('/');
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
