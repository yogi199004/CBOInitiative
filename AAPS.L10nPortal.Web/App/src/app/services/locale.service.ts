import { Injectable } from '@angular/core';
import { ILocale } from '../entities/locale.interface';
import { LocaleRepository } from '../repositories/locale.repository';

@Injectable()
export class LocaleService {
  constructor(
    private localeRepository: LocaleRepository
  ) { }

  getAvailablesLocalesAsync(): Promise<ILocale[]> {
    return this.localeRepository.getAllLocalesAsync();
  }
}
