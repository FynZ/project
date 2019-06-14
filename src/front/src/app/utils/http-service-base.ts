import { HttpHeaders } from '@angular/common/http';

export abstract class HttpServiceBase
{
    protected jsonHeaders: HttpHeaders = new HttpHeaders({
        'Content-Type': 'application/json'
    });

    constructor() { }
}
