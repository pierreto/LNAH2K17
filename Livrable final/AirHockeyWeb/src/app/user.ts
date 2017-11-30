export class User {

    constructor(private username?: string, private password?: string, private email?: string,
                private name?: string, private profile?: string) {
        this.username = '';
        this.password = '';
        this.email = '';
        this.name = '';
        this.profile = '';
    }

    public get Username(): string {
        return this.username;
    }

    public get Password(): string {
        return this.password;
    }

    public get Name(): string {
        return this.name;
    }

    public get Email(): string {
        return this.email;
    }

    public get Profile(): string {
        return this.profile;
    }
}
