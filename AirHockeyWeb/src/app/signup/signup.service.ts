import { User } from './../user';
import { Injectable } from '@angular/core';

@Injectable()
export class SignupService {
    signup(user: User) {
        console.log('TODO: Implement signup', user);
        // TODO: Get username, if not found, post
    }
}
