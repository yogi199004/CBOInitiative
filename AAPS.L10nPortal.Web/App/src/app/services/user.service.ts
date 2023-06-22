import { Injectable } from '@angular/core';
import { IUser } from '../entities/user.interface';
import { UserRepository } from '../repositories/user.repository';

@Injectable()
export class UserService {
  constructor(
    private userRepository: UserRepository
  ) {
   // this.init();
  }

  private currentUserPromise: Promise<IUser> = null;

  private init(): void {
    this.updateCurrentUserPromise();
  }

  private updateCurrentUserPromise(): void {
    this.currentUserPromise = this.userRepository.getCurrentUserAsync();
  }

  getCurrentUserAsync(forceGetUser: boolean = false): Promise<IUser> {
    if (forceGetUser) {
      this.updateCurrentUserPromise();
    }

    return this.currentUserPromise;
  }

  renewSessionAsync(): Promise<void> {
    return this.userRepository.renewSessionAsync();
  }
  getSuperadminAccessdata(): Promise<boolean> {
    return this.userRepository.getSuperadminAccessdata();
  }
}
