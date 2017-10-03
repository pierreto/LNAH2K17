export class User {

    constructor(private username?: string, private password?: string) {
        this.username = '';
        this.password = '';
    }

    public get Username(): string {
        return this.username;
    }

    public get Password(): string {
        return this.password;
    }
}
