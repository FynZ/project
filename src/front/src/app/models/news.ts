export class News
{
    public title: string;
    public content: string;
    public creationDate: Date;

    constructor(title: string, content: string, creationDate: Date)
    {
        this.title = title;
        this.content = content;
        this.creationDate = creationDate;
    }
}
