export class NewsDetails
{
    public id: number;
    public title: string;
    public slug: string;
    public content: string;
    public userCreationId: number;
    public creationDate: Date;
    public modificationDate: Date;
    public comments: Comment[];
}
