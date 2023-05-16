import { MilisecondsToMinutesPipe } from './miliseconds-to-minutes.pipe';

describe('MilisecondsToMinutesPipe', () => {
  it('create an instance', () => {
    const pipe = new MilisecondsToMinutesPipe();
    expect(pipe).toBeTruthy();
  });
});
