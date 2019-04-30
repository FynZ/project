export class Token
{
    public id: number;
    public name: string;
    public iss: string;
    public sub: number;
    public aud: string;
    public exp: number;
    public nbf: number;
    public iat: number;
    public jti: string;
    public roles: string[];
}
