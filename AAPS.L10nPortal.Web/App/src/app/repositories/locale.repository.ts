import { Injectable } from '@angular/core';
import { ILocale } from '../entities/locale.interface';
import { BaseRepository } from './base.repository';

@Injectable()
export class LocaleRepository extends BaseRepository {
  getAllLocalesAsync(): Promise<ILocale[]> {
    return this.get<ILocale[]>('/api/Locale');
  }
}
