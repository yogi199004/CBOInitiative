import { MilisecondsToSecondsPipe } from './miliseconds-to-seconds.pipe';

describe('MilisecondsToSecondsPipe', () => {
  it('create an instance', () => {
    const pipe = new MilisecondsToSecondsPipe();
    expect(pipe).toBeTruthy();
  });
});
