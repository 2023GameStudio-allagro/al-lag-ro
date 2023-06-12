public interface IScoreManager
{
	ScoreData scoreData{get;}
	void HitEnemy(int count);
	void HitEnemyPerfect(int count);
	void AttackWrongTime();
	void Miss();
	void GetDamagedByEnemy();
}
