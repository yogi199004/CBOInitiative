import { Injectable } from '@angular/core';
import { IUser } from '../entities/user.interface';
import { BaseRepository } from './base.repository';

@Injectable()
export class UserRepository extends BaseRepository {
 

  renewSessionAsync(): Promise<void> {
    return this.get<void>('/api/User/RenewSession');
  }
  getSuperadminAccessdata(): Promise<boolean> {
    return this.get<boolean>("/api/User/AdminUser")
  }
}


